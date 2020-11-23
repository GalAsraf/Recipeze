import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CategoriesComponent } from './components/categories/categories.component';
import { CookbookComponent } from './components/cookbook/cookbook.component';
import { CurrentRecipeComponent } from './components/current-recipe/current-recipe.component';
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
  { path: 'recipes/:search/:whatChecked', component: RecipesComponent },
  { path: 'current-recipe/:recipe', component: CurrentRecipeComponent },
  { path: 'cookbook', component: CookbookComponent }
  //{ path: '**', component: PageNotFoundComponent }
  //,treatSens - add it to the path, to send also the sensitive
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
