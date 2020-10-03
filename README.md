# Bot.CommonServices
Some common services that can be used to build bots with Mirosoft Bot Framework

## Motivation
Building a bot from scratch is hard. All human has some common capbilities like see, hear, smell etc. A bot should have some fundamental functionalilies built in as well. In this repo, I hope we can build up a toolbox of individual modules. In the future, if you want to build a bot that can take some order from customers, but you want your bot to feel like human and chit chat a little, like check weather, or tell a joke, or understand the image the customer send you. You can plugin these modules and play.

## Tasks
I think we should build these common modules. The list is not based on priorities, some of them are implemented first because I've used them in my previous projects.

- [x] Vision Service - Understand the image, detects objects/celebrities, read text from the image. I've build a service using microsofot vision API, you can help build more implemations by connecting to other APIs.
- [x] QR Code - I've created two implementaitons of the QR decoders, it can decode a normal QR code or colored ones generated ColorZXing.Net RGB mode.
- [ ] Weather Service. 
- [ ] Dictionary Service.
- [ ] News Service. 
- [ ] Joke Service.

## Design
### Interface
These services should be defined as interfaces, and we can build multiple implementations of these interfaces.
e.g. IVisionService

```
public Task<OcrResult> ReadTextFromImageAsyc(string imageUrl);
public Task<T> ReadTextFromImageAsyc<T>(string imageUrl, IConverter<OcrResult, T> beautifier);
```
For each feature, I think we should create two functions, one returns a objec which user can dig into the details implement their own logic. The second one takes in a Converter which returns a converted object. Some common scenrios could be you want to generate a Hero card directly, or you just want to get the text result. You can also implement your own converter to create your own objects based on the interface result.

### Classes
Whenever applicable, a class should be able to be initialized using dependency injection or by passing in the parameters directly.
e.g. The AzureVisionService 
```
public AzureVisionService(IConfiguration configuration)
public AzureVisionService(string endpoint, string key)
```

## Releases
This project is still at the early stage, but you can try it using the sample nuget package. https://www.nuget.org/packages/Bot.CommonServices/
```
Install-Package Bot.CommonServices
```

#### ------------------------------------------------------------------------------------------------------------------------
Feel free to reach me through email (hainan.zhao@live.com), or we can arrange some calls to discusse more details. All advice/feedback/contributions are appreciated.


