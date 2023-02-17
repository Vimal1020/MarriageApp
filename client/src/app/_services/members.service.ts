import { JsonPipe } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/Member';

const httpOptions = {
  headers: new HttpHeaders({
    Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))
  })
}



var header = {
  headers: new HttpHeaders()
    .set('Authorization',  `Basic ${btoa(localStorage.getToken())}`)
}

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { 
    
  }
 
  getMembers() {
    var header2=JSON.parse(localStorage.getItem('user'))
    return this.http.get<Member[]>(this.baseUrl + 'users',header);
  }
   getMember(username){
    return this.http.get<Member>(this.baseUrl+'users/'+ username,httpOptions);
   }
}
