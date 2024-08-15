import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavbarComponent } from './navbar/navbar.component';
import { FooterComponent } from './footer/footer.component';
import { HomeComponent } from './home/home.component';
import { PlayComponent } from './play/play.component';
import { SharedModule } from './shared/shared.module';
import { HTTP_INTERCEPTORS, provideHttpClient } from '@angular/common/http';
import { JwtInterceptor } from './shared/interceptors/jwt.interceptor';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideNativeDateAdapter } from '@angular/material/core';
@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FooterComponent,
    HomeComponent,
    PlayComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    SharedModule,
    BrowserAnimationsModule,
    FontAwesomeModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true,
    },
    provideHttpClient(),
    provideAnimationsAsync(),
    provideNativeDateAdapter(),
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
