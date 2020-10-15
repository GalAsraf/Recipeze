import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddUserComponent } from './components/add-user/add-user.component';
import { CategoriesComponent } from './components/categories/categories.component';
import { DefineAllergyComponent } from './components/define-allergy/define-allergy.component';


const routes: Routes = [
  { path: '', component: CategoriesComponent },
 // { path: 'category', component: CategoriesComponent },
  { path: 'add-user', component: AddUserComponent },
  { path: 'allergies', component: DefineAllergyComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
