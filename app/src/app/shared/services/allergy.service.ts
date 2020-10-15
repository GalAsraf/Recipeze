import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Allergy } from '../models/allergy.model';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AllergyService {

  constructor(private http: HttpClient) { }
  
  getAllAllergies():Observable<Allergy[]>
  {
    return this.http.get<Allergy[]>(environment.url+"allergy/getAllAllergies")

  }
}
