import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import {CollapseModule} from 'ngx-bootstrap/collapse';
import {TooltipModule} from 'ngx-bootstrap/tooltip';
import {BsDropdownModule} from 'ngx-bootstrap/dropdown';
import {ModalModule} from 'ngx-bootstrap/modal';
import {PaginationModule} from 'ngx-bootstrap/pagination';

import {BsDatepickerModule} from 'ngx-bootstrap/datepicker';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';


import { EventoService } from './services/evento.service';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgxCurrencyModule } from 'ngx-currency';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './shared/nav/nav.component';
import { EventosComponent } from './componentes/eventos/eventos.component';
import { PalestrantesComponent } from './componentes/palestrantes/palestrantes.component';
import { DateTimeFormatPipe } from './helpers/DateTimeFormat.pipe';
import { ContatosComponent } from './componentes/contatos/contatos.component';
import { DashboardComponent } from './componentes/dashboard/dashboard.component';
import { PerfilComponent } from './componentes/user/perfil/perfil.component';
import { TituloComponent } from './shared/titulo/titulo.component';
import { EventoDetalheComponent } from './componentes/eventos/evento-detalhe/evento-detalhe.component';
import { EventoListaComponent } from './componentes/eventos/evento-lista/evento-lista.component';
import { UserComponent } from './componentes/user/user.component';
import { LoginComponent } from './componentes/user/login/login.component';
import { RegistrationComponent } from './componentes/user/registration/registration.component';
import { LoteService } from './services/lote.service';
import { AccountService } from './services/account.service';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { HomeComponent } from './componentes/home/home.component';

defineLocale('pt-br', ptBrLocale);

@NgModule({
  declarations: [
    AppComponent,
    EventosComponent,
    PalestrantesComponent,
    NavComponent,
    DateTimeFormatPipe,
    ContatosComponent,
    DashboardComponent,
    PerfilComponent,
    TituloComponent,
    EventoDetalheComponent,
    EventoListaComponent,
    HomeComponent,
    UserComponent,
    LoginComponent,
    RegistrationComponent
   ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    CollapseModule.forRoot(),
    FormsModule,
    ReactiveFormsModule,
    TooltipModule.forRoot(),
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ModalModule.forChild(),
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      progressBar: true
    }),
    NgxSpinnerModule,
    NgxCurrencyModule,
    PaginationModule.forRoot()
  ],
  providers: [
    EventoService,
    LoteService,
    AccountService,
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
