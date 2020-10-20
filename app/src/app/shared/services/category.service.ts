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

  googleSearch(categories: string): Observable<boolean> {
    return this.http.post<boolean>(environment.url + 'category/getSelectedCategories', categories)
  }

}
