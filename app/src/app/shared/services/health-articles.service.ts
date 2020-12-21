import { Injectable } from '@angular/core';
import { healthArticles } from '../models/healthArticles.modal';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class HealthArticlesService {

  constructor(private http: HttpClient) { }

  getRandomArticles(): Observable<healthArticles[]> {
    return this.http.get<healthArticles[]>(environment.url + 'healthArticles/GetRandomArticles');
  }
}

