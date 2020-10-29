import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Category } from '../models/category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient) { }

  getAllCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(environment.url + "category/getAllCategories")
  }

//  googleSearch(categories: Category[]): Observable<boolean> {

   googleSearch(categories: string): Observable<string> {
     let userId=localStorage.getItem('currentUser');
     return this.http.get<string>(environment.url + 'category/getSelectedCategories/'+categories)
   }
/* 
  googleSearch(categories: string): Observable<string> {
    let userId=localStorage.getItem('currentUser');
    return this.http.post<string>(environment.url + 'category/getSelectedCategories/'+userId,categories);
  }
 */
  
}
