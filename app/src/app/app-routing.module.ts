import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CategoriesComponent } from './components/categories/categories.component';
import { DefineAllergyComponent } from './components/define-allergy/define-allergy.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RecipesComponent } from './components/recipes/recipes.component';
import { RegisterComponent } from './components/register/register.component';


const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'home', component: HomeComponent },
  { path: 'category', component: CategoriesComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: 'allergies', component: DefineAllergyComponent },
  { path: 'recipes/:search', component: RecipesComponent }
  //,treatSens - add it to the path, to send also the sensitive
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
