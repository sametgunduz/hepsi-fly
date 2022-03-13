FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 4040
COPY ./publish .
ENV ASPNETCORE_URLS=http://+:4040
ENTRYPOINT ["dotnet", "HepsiFly.Api.dll"]