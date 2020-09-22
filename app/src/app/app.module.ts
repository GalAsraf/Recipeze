import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import {FormsModule,ReactiveFormsModule} from '@angular/forms'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { HttpClientModule } from '@angular/common/http';
import { AddUserComponent } from './components/add-user/add-user.component';
<<<<<<< HEAD
import { LoginComponent } from './components/login/login.component';
=======
import { CategoriesComponent } from './components/categories/categories.component';
>>>>>>> 5a28c109d0b335736527ca60efdf93d28fadedab

@NgModule({
  declarations: [
    AppComponent,
    AddUserComponent,
<<<<<<< HEAD
    LoginComponent,
=======
    CategoriesComponent,
>>>>>>> 5a28c109d0b335736527ca60efdf93d28fadedab
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent,AddUserComponent]
})
export class AppModule { }
