import { NestFactory, Reflector } from '@nestjs/core';
import { AppModule } from './app.module';
import { PORT } from './constants/port.constants';
import { CORS } from './constants/cors.constants';
import { DocumentBuilder, SwaggerModule } from '@nestjs/swagger';
import * as morgan from "morgan"
import * as cookieParser from "cookie-parser"
import { GloablExeptionsFilter } from './utils/globalFilterError.managger';
import { ClassSerializerInterceptor, ValidationPipe } from '@nestjs/common';

async function bootstrap() {
  const app = await NestFactory.create(AppModule);

  app.use(morgan("dev"))
  app.use(cookieParser())
  app.useGlobalFilters(new GloablExeptionsFilter())

  app.enableCors(CORS)
  app.setGlobalPrefix("api")

  app.useGlobalPipes(new ValidationPipe({
    transformOptions: {
      enableImplicitConversion: true
    }
  }))
  const reflector = app.get(Reflector)
  app.useGlobalInterceptors(new ClassSerializerInterceptor(reflector))

  const config = new DocumentBuilder()
    .setTitle("Auth Test")
    .setDescription("Test by passport, jwt and cookie")
    .setVersion("1.9")
    .addBearerAuth()
    .build()

  const document = SwaggerModule.createDocument(app, config)
  SwaggerModule.setup("docs", app, document)

  await app.listen(PORT);
  console.log(`app listen in port ${PORT}`)
}

bootstrap();
