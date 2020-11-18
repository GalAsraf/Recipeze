import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Recipe } from '../models/recipe.model';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

import { Allergy } from '../models/allergy.model';
import { Category } from '../models/category.model';


@Injectable({
  providedIn: 'root'
})
export class RecipeService {

  constructor(private http: HttpClient) { }

  addRecipeToCookbook(recipe:Recipe): Observable<boolean>{
    let userId = localStorage.getItem('currentUser');
    return this.http.post<boolean>(environment.url + 'recipe/addRecipeToCookbook/'+userId,recipe);
  }

  getUserCookbook(): Observable<Recipe[]>{
    let userId = localStorage.getItem('currentUser');
    return this.http.post<Recipe[]>(environment.url + 'recipe/getUserCookbook/',userId);
  }
}
