# HepsiFly Case Study

## Contens
- Restful Service
- Microservices Topology
- CI/CD Yaml Files

## Case 1 : (Restful Service)
* Uygulama içerisinde category ve product şeklinde 2 farklı resource bulunmaktadır. 
* Uygulama içerisinde **MediatR,MongoDB.Bson,StackExchange.Redis,FluentValidaton** gibi çeşitli package'lar kullanılmıştır. 
* Uygulamada resource'ların data etkileşimleri için **repository pattern** uygulanmıştır.
* Uygulamada **CQRS** pattern uygulanmış ve command-query'ler ayrıştırılmıştır. **Uygulama içerisinde sadece mongodb bulunmaktadır fakat normal şartlarda write için mssql query işlemleri içinse mongodb kullanıldığı varsayılmıştır.**
* Product get servisinde redis implemente edilmiş fakat product'ın güncellenmesi, silinmesi gibi işlemlerin sonrasındaki cache handler'ları yazılmış varsayılmıştır.
* Uygulamanın bir study-case olmasından dolayı ilgili resource entitiyler direk response model olarak verilmiştir herhangi bir dto vb. transfer objeleri kullanılmamıştır.

#### Installation api
```bash
dotnet restore
dotnet publish -o ./publish

docker build -t hepsiflyapi . 
docker-compose up
```

>İlgili servise `http://localhost:4040` üzerinden erişilebilir.
 Dökümantasyon için `http://localhost:4040/swagger` a gidilebilir.



## Case 2: (Microservices Topology)

Bu case'e ait kaynak çizim repo altında 'HepsiFly/Sources/hepsifly_ms_topology.html' dizinindedir.
- Dışarıdan gelen istekler yine uygulama ile aynı datacenter'da bulunan bir **load balancer** ile **istio apigateway** e iletilir ve buradan da istio apigateway **routing** ine göre ilgili **virtual service**'e yönlendirme yapılır.
- Uygulama genel itibariyle **Kubernates** üzerinde çalışmaktadır. 
- **ns:hepsifly** namespace'i altında ilgili microservice'ler bulunmaktadır. 
- a ve b service'lerinin çok istek alması hem mesajlaşmada hemde cache tarafında high availability sağlayabilmek için **ns:rabbit** ve **ns:redis** clusterları oluşturulmuştur.
- Yapı içerisine **high availability**'nin sağlanabilmesi için **auto-scaling** yöntemi uygulanmış ve burada **Keda Scaler** ve **Prometheus** gibi araçlardan faydalanılmıştır.
- Uygulama içerisindeki mesajlaşma event-based olduğundan hem **event-based bir auto-scaling** hemde **http request bazlı metriklere göre** auto-scaling uygulayabilmek için **KEDA + Prometheus** tercih edilmştir. 
- Senaryoya göre hem helen http isteklere hemde event yoğunluğuna göre podlar scale edilmektedir. 
- Bu yöntemleri tercih etmeyip basit olarak **k8s**'in **HPA** altyapısını kullanarakta (pod cpu ve ram'e bakarak) auto-scaling yapabiliriz.
- Ölçeklenebilir bir data katmanı için ilgili database'ler cluster dışında tutulmuştur.
- Topolojide yer verilmesede böyle bir uygulamada distributed tracing için **jeager**, structured loggin için fluent bir **elasticsearch ve kibana**, metrics içinse **prometeus ve grafana** tercih edilebilir.

## Case 3: (CI/CD YAMLs)
Bu case'e ait kaynak dosyalar repo altında 'HepsiFly/Sources/' dizinindedir.

- #### bitbucket-ci.yaml
 Bitbucket üzerinden ci yapmayı sağlayan yaml dosyasıdır. **master** ve **development** branch'leri için ilgili pipeline çalıştırır.

- #### a-service-deployment.yaml 
Buradaki yaml dosyamız fazla istek alan a-service(prod)'a aittir. 
Yoğun istek alan bir service olmasına istinaden bu service deployment tanımlarındaki **resource** ve **request** tanımları buna yönelik tanımlanmıştır.
Yine deployment stratejisi olarak **RollingUpdate** tercih edilmiştir. 
Bu sayede rolling update oranı ile yeni versiyon’a sahip podları ayağa kaldırılırken eski versiona ait podlar kaldırılır ve bir kesinti yaşanmamış olur.
Aynı zamanda yaml file'ın son adımında auto-scaling'de eklenmiştir.

- #### a-service-dev-deployment.yaml
Buradaki yaml dosyamız fazla istek alan a-service(development)'a aittir.
Development ortamı olduğundan dolayı kulanılan **kaynak miktarları daha düşüktür** ve deployment stratejisi olarak **Recreate** tercih edilmiştir. 

#### Buraya daha az istek alan b-service'e ait yaml dosyaları eklenmemiştir. b-service için a-service yapılandırması uygulanabilir, az istek alan bir service olduğundan dolayı ilgili kaynak miktarları ve deployment statejileri ona göre ayarlanabilir.

Teşekkürler. :blush:
