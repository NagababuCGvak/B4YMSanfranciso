<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections >
    <sectionGroup name="zahir">
      <section name="logging" type="Zahir.Library.Logging.Configuration.LoggingConfigurationSection, Zahir.Library.Logging"/>
    </sectionGroup>
  </configSections>

  <zahir>
    <logging default="dbLogger">
      <logger name="dbLogger" type="Zahir.Library.Logging.DbLogger, Zahir.Library.Logging" nextLogger="textFile">
          <constructorParameter name="connectionStringName" value="LogDatabase"/>
      </logger>
      <logger name="console" type="Zahir.Library.Logging.ConsoleLogger, Zahir.Library.Logging" nextLogger="null"/>
      <logger name="null" type="Zahir.Library.Logging.NullLogger, Zahir.Library.Logging"/>
      <logger name="textFile" type="Zahir.Library.Logging.TextFileLogger, Zahir.Library.Logging">
        <constructorParameter name="filePath" value="c:\LogFile.txt"/>
      </logger>

      <logger name="mail" type="Zahir.Library.Logging.MailLogger, Zahir.Library.Logging" nextLogger="dbLogger">
        <constructorParameter name="mailAddress" value="smtp@hizmet.web.tr "/>
        <constructorParameter name="password" value="123456"/>
        <constructorParameter name="to" value="zahirsolak@gmail.com"/>
      </logger>      
    </logging>
  </zahir>
</configuration>