FROM microsoft/dotnet:2.0.0-preview1-runtime
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "Medico.Core.Patients.dll"]
