# Search Job App
> Search Job App bir iş ilanı yayınlama ve listeleme uygulamasıdır. Uygulamanın amacı iş ilanı yayınlayabilen (işveren) kullanıcı olarak kayıt oluşturup, ilan yayınlayabileceği ve
  ilanları görüntüleyebileceği back-end altyapının sağlanmasıdır.

# Tech Stack
* .NET 6 PostgreSQL EntityFramework Core ElasticSearch NEST MediatR AutoMapper FluentValidation BCrypt JWT Docker 

# Mimari ve Patternler
* Onion Architecture
* CQRS Pattern
* Mediator Pattern

# Kurulum

Uygulamayı local'de çalıştırmak isterseniz, docker-compose up ya da terminalden detach olarak container'ları ayağa kaldırmak isterseniz docker-compose up -d komutuyla tüm ekosistemi ayağa kaldırabilirsiniz.


```yml
version: '3.8'

services:
  s_searchjobapp_postgres:
    hostname: postgres
    container_name: c_searchjobapp_postgres
    image: postgres
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres@123
    volumes:
      - ./init.sql:/docker-entrypoint-initdb.d/init.sql
    ports:
      - "5432:5432"
    networks:
      - searchjobapp-backend
    restart: always

  s_searchjobapp_elasticsearch:
    hostname: elasticsearch
    container_name: c_searchjobapp_elasticsearch
    image: elasticsearch:7.17.10
    volumes:
      - searchjobapp_elasticsearch_data:/usr/share/elasticsearch/data
    environment:
      bootstrap.memory_lock: true
      ES_JAVA_OPTS: "-Xmx512m -Xms512m"
      discovery.type: single-node
      ELASTIC_PASSWORD: elastic@123
    ports:
      - "9300:9300"
      - "9200:9200"
    networks:
      - searchjobapp-backend
    restart: always

  s_searchjobapp_api:
    hostname: searchjobapp
    container_name: c_searchjobapp_api
    image: searchjobappapi
    environment:
      - ASPNETCORE_URLS=http://+:5130
      - ASPNETCORE_ENVIRONMENT=Production
    build:
      context: .
      dockerfile: src/SearchJobApp.Api/Dockerfile
    ports:
      - "5130:5130"
    networks:
      - searchjobapp-backend
    restart: always

networks:
  searchjobapp-backend:
    driver: bridge

volumes:
  searchjobapp_rabbitmq_data:
  searchjobapp_elasticsearch_data:
```

```
docker-compose up
```

# Kullanım

### Domain Bilgisi

> Employers ve Posts olmak üzere iki adet tablomuz var. İşveren kayıtları Employers tablosunda, iş ilanı kayıtları da Posts tablosunda tutuluyor.
> İşveren ve iş ilanı kayıtları Employer.Id ve Post.EmployerId alanlarıyla eşleşiyor. Her ilanın bir işvereni olmak zorundadır.

#### Entities

```cs
public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}

public class Employer : BaseEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Title { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public int RemainingPostingQuantity { get; set; }
}

public class Post : BaseEntity
{
    public Guid EmployerId { get; set; }
    public string EmployerTitle { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public int? QualityScore { get; set; }
    public string? AdditionalMessage { get; set; }
    public WorkType? WorkType { get; set; }
    public PositionLevel? PositionLevel { get; set; }
    public string? Salary { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
```

#### Enums

```cs
public enum WorkType
{
    FullTime = 1,
    PartTime = 2,
    FreeTime = 3,
    Periodic = 4
}

public enum PositionLevel
{
    Intern = 1,
    Beginner = 2,
    Specialist = 3,
    SeniorSpecialist = 4,
    Manager = 5
}
```

### İşveren Kaydı

* Url: http://localhost:5130/api/Employer
* Method: POST
* Response: `Guid Employer.Id`

> Uygulamayı kullanabilmek için ilk önce işveren kaydı yapmamız gerekiyor. Email, Password, Title, Phone ve Address alanları zorunlu. 
> Email alanında email validasyonu ve phone alanında da 90 ile başlayıp 12 haneli numara validasyonu bulunuyor.

* Request:

```curl
curl -X 'POST' \
  'http://localhost:5130/api/Employer' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
    "email": "mustafa.cinar@kariyer.net",
    "password": "mustafa@1903",
    "title": "Kariyer.Net",
    "phone": "905555555555",
    "address": "AVCILAR / İSTANBUL"
}'
```

* Response:
```json
"845e5ed9-794b-46c5-8b08-899d44d6c774"
```

### Sisteme Giriş

* Url: http://localhost:5130/api/auth
* Method: POST
* Response: `string AccessToken`

> İlan yayınlayabilmek için işveren olarak authenticate olmamız gerekli. Email, Password alanları zorunlu.

* Request: 

```curl
curl -X 'POST' \
  'http://localhost:5130/api/Auth' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
    "email": "mustafa.cinar@kariyer.net",
    "password": "mustafa@1903"
}'
```

* Response:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbXBsb3llcklkIjoiODQ1ZTVlZDktNzk0Yi00NmM1LThiMDgtODk5ZDQ0ZDZjNzc0IiwibmJmIjoxNjg3MTYxMjQ1LCJleHAiOjE2ODc0MjA0NDUsImlhdCI6MTY4NzE2MTI0NX0.-NGQY-6VyXsz6eIVXHy4jrcNQpQpjHRSHNZl5LyFF2Q"
}
```

### İlan Oluşturmak

* Url: http://localhost:5130/api/post
* Method: POST
* Response: `Guid Post.Id`

> İlan yayınlayabilmek için işveren olarak authorize olmamız gerekli. Yukarıdaki auth endpointinden aldığımız access token'ı request header'ına eklememiz gerekiyor.
> Title, Message alanları zorunlu. WorkType ve PositionLevel alanlarını da yukarıda belirtilen enum type'lara göre isteğe bağlı doldurulabilir.

* Request:

```curl
curl -X 'POST' \
  'http://localhost:5130/api/Post' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbXBsb3llcklkIjoiODQ1ZTVlZDktNzk0Yi00NmM1LThiMDgtODk5ZDQ0ZDZjNzc0IiwibmJmIjoxNjg3MTYxMjQ1LCJleHAiOjE2ODc0MjA0NDUsImlhdCI6MTY4NzE2MTI0NX0.-NGQY-6VyXsz6eIVXHy4jrcNQpQpjHRSHNZl5LyFF2Q' \
  -d '{
  "title": "Software Engineer",
  "message": "Software Engineer İş İlanı",
  "additionalMessage": "Software Engineer İş İlanı Yan haklar ek bilgiler vs."
  "workType": 1,
  "positionLevel": 3,
  "salary": null
}'
```

* Response:
```json
"ddac9b08-e7b2-4240-8185-9e9c8d3abfd2"
```

### İlanları Listelemek

* Url: http://localhost:5130/api/post
* Method: GET
* Response: `List<PostDto>`

> Sistemdeki yayınlanmış fakat yayında kalma süresi geçmemiş tüm ilanları listeler. Yayında kalma süresi ilan yayınlandıktan 15 gün sonrasıdır.


* Request:

```curl
curl -X 'GET' \
  'http://localhost:5130/api/Post' \
  -H 'accept: */*'
```

* Response:
```json
[
  {
    "id": "dcbda8e3-52b9-4ac1-9d9d-82dd936230f7",
    "employerId": "2f6e8e75-aebe-4270-b240-7a34bdbe7c95",
    "employerTitle": "Kariyer.Net",
    "title": "Software Engineer",
    "message": "Software Engineer İş İlanı",
    "qualityScore": 3,
    "additionalMessage": "Software Engineer İş İlanı Yan haklar ek bilgiler vs.",
    "workType": "FullTime",
    "positionLevel": "Specialist",
    "salary": null,
    "startDate": "2023-06-19T06:57:25.2849873+00:00",
    "endDate": "2023-07-04T06:57:25.2849727+00:00",
    "createdDate": "2023-06-19T06:57:25.2849985+00:00",
    "modifiedDate": "2023-06-19T06:57:25.2850089+00:00"
  }
]
```

### İşverene Ait İlanları Listelemek

* Url: http://localhost:5130/api/{employerId}/posts
* Method: GET
* Response: `List<EmployerWithPostsDto>`

> Sistemdeki işverene ait yayınlanmış fakat yayında kalma süresi geçmemiş tüm ilanları işveren bilgisiyle beraber listeler. Yayında kalma süresi ilan yayınlandıktan 15 gün sonrasıdır.


* Request:

```curl
curl -X 'GET' \
  'http://localhost:5130/api/Employer/2f6e8e75-aebe-4270-b240-7a34bdbe7c95/posts' \
  -H 'accept: */*'
```

* Response:
```json
{
  "posts": [
    {
      "id": "dcbda8e3-52b9-4ac1-9d9d-82dd936230f7",
      "employerId": "2f6e8e75-aebe-4270-b240-7a34bdbe7c95",
      "employerTitle": "Kariyer.Net",
      "title": "Software Engineer",
      "message": "Software Engineer İş İlanı",
      "qualityScore": 3,
      "additionalMessage": "Software Engineer İş İlanı Yan haklar ek bilgiler vs.",
      "workType": "FullTime",
      "positionLevel": "Specialist",
      "salary": null,
      "startDate": "2023-06-19T06:57:25.2849873+00:00",
      "endDate": "2023-07-04T06:57:25.2849727+00:00",
      "createdDate": "2023-06-19T06:57:25.2849985+00:00",
      "modifiedDate": "2023-06-19T06:57:25.2850089+00:00"
    }
  ],
  "id": "2f6e8e75-aebe-4270-b240-7a34bdbe7c95",
  "email": "mustafa.cinar@kariyer.net",
  "title": "Kariyer.Net",
  "phone": "905555555555",
  "address": "AVCILAR / İSTANBUL",
  "remainingPostingQuantity": 1,
  "createdDate": "2023-06-19T06:17:39.836353+00:00",
  "modifiedDate": "2023-06-19T06:17:39.8363758+00:00"
}
```

### İşveren Bilgisi Görüntüleme

* Url: http://localhost:5130/api/employer/{employerId}
* Method: GET
* Response: `EmployerDto`

> Sistemdeki işveren bilgisini görüntüler.


* Request:

```curl
curl -X 'GET' \
  'http://localhost:5130/api/Employer/2f6e8e75-aebe-4270-b240-7a34bdbe7c95' \
  -H 'accept: */*'
```

* Response:
```json
{
  "id": "2f6e8e75-aebe-4270-b240-7a34bdbe7c95",
  "email": "mustafa.cinar@kariyer.net",
  "title": "Kariyer.Net",
  "phone": "905555555555",
  "address": "AVCILAR / İSTANBUL",
  "remainingPostingQuantity": 1,
  "createdDate": "2023-06-19T06:17:39.836353+00:00",
  "modifiedDate": "2023-06-19T06:17:39.8363758+00:00"
}
```

### İlan Aramak

* Url: http://localhost:5130/api/Post/search
* Method: GET
* Response: `List<PostDto>`

> Sistemdeki kayıtlı ilanlarda arama yapar. Yukarıda belirtilen enum type'larda karşılık gelen değere göre aralarına virgül koyarak çoklu arama yapabilirsiniz.
> Örnek vermek gerekirse, çalışma şekli full time ya da part time ilanları aramak istiyorsak enum type'daki karşılık gelen 1 ve 2 değerlerini, workType=1,2 şeklinde query parametresi gönderebiliriz.


* Request:

```curl
curl -X 'GET' \
  'http://localhost:5130/api/Post/search?searchText=kariyer&workType=1,2&positionLevel=1,2,3' \
  -H 'accept: */*'
```

* Response:
```json
[
  {
    "id": "dcbda8e3-52b9-4ac1-9d9d-82dd936230f7",
    "employerId": "2f6e8e75-aebe-4270-b240-7a34bdbe7c95",
    "employerTitle": "Kariyer.Net",
    "title": "Software Engineer",
    "message": "Software Engineer İş İlanı",
    "qualityScore": 3,
    "additionalMessage": "Software Engineer İş İlanı Yan haklar ek bilgiler vs.",
    "workType": "FullTime",
    "positionLevel": "Specialist",
    "salary": null,
    "startDate": "2023-06-19T06:57:25.2849873+00:00",
    "endDate": "2023-07-04T06:57:25.2849727+00:00",
    "createdDate": "2023-06-19T06:57:25.2849985+00:00",
    "modifiedDate": "2023-06-19T06:57:25.2850089+00:00"
  }
]
```
