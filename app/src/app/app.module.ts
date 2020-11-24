import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from '@angular/forms'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { HttpClientModule } from '@angular/common/http';
import { CategoriesComponent } from './components/categories/categories.component';
import { DefineAllergyComponent } from './components/define-allergy/define-allergy.component';
import { AllergyService } from './shared/services/allergy.service';
import { CategoryService } from './shared/services/category.service';
import { UserService } from './shared/services/user.service';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { RecipesComponent } from './components/recipes/recipes.component';
import { CookbookComponent } from './components/cookbook/cookbook.component';
import { CurrentRecipeComponent } from './components/current-recipe/current-recipe.component';
import { DynamicDialogModule } from 'primeng/dynamicdialog';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


import { ToastModule } from 'primeng/toast';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DataViewModule } from 'primeng/dataview';


//import {DataViewModule} from 'primeng/dataview';


//import {AccordionModule} from 'primeng/accordion';     //accordion and accordion tab
//import {MenuItem} from 'primeng/api';                  //api

@NgModule({
  declarations: [
    AppComponent,
    CategoriesComponent,
    DefineAllergyComponent,
    RegisterComponent,
    LoginComponent,
    HomeComponent,
    RecipesComponent,
    CookbookComponent,
    CurrentRecipeComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    HttpClientModule,
    DynamicDialogModule,
    ToastModule,
    TableModule,
    ButtonModule,
    BrowserAnimationsModule,
    DataViewModule
  ],
  entryComponents: [
    CurrentRecipeComponent
  ],
  providers: [AllergyService, CategoryService, UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
