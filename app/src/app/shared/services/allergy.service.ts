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
  defineAllergiesForUser(selectedAllergies: number[]) :Observable<boolean>{
    let userId=localStorage.getItem('currentUser');
    return this.http.post<boolean>(environment.url+"allergy/defineAllergiesForUser/"+userId,selectedAllergies)
  }

  getCurrentUserAllergies():Observable<Allergy[]>
  {
    let userId=localStorage.getItem('currentUser');
    return this.http.get<Allergy[]>(environment.url+"allergy/getCurrentUserAllergies/"+userId)
  }

/* 
  defineAllergies(allergies: string): Observable<string> {
    return this.http.post<string>(environment.url + 'allergy/AddAllergie/'+allergies)
  } */
}
