import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Allergy } from '../models/allergy.model';
import { Category } from '../models/category.model';
import { Recipe } from '../models/recipe.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient) { }

  getAllCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(environment.url + "category/getAllCategories")
  }

  searchText(searchText: string): Observable<string> {
    return this.http.get<string>(environment.url + 'category/getSearchText/'+ searchText);
  }
  


  googleSearch(categories: string, whatChecked: number[]): Observable<Recipe[]> {
    let userId=localStorage.getItem('currentUser');
    return this.http.post<Recipe[]>(environment.url + 'category/getSelectedCategories/'+userId+'/'+categories,whatChecked);
  }
 
  
}
