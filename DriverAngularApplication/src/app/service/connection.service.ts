import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ConnectionService {
  apiUrlPrefix = environment.apiUrlPrefix;

  constructor(private http: HttpClient) {}

  post(url: string, body: any): Observable<any> {
    const fullUrl: string = this.apiUrlPrefix + url;
    return this.http.post(fullUrl, body);
  }

  patch(url: string, body: any): Observable<any> {
    const fullUrl: string = this.apiUrlPrefix + url;
    return this.http.patch(fullUrl, body);
  }

  get(url: string, param?: HttpParams): Observable<any> {
    const fullUrl: string = this.apiUrlPrefix + url;
    const opts = param ? { params: param } : {};
    return this.http.get(fullUrl, opts);
  }

  put(url: string, body: any): Observable<any> {
    const fullUrl: string = this.apiUrlPrefix + url;
    return this.http.put(fullUrl, body);
  }

  delete(url: string): Observable<any> {
    const fullUrl: string = this.apiUrlPrefix + url;
    return this.http.delete(fullUrl);
  }
}
