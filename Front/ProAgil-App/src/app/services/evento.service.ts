import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Evento } from '../models/Evento';

@Injectable(
  //{providedIn: 'root'}
  )
export class EventoService {

  constructor(private http: HttpClient) { }

  baseURL = environment.apiURL + 'api/eventos';


  public getEventos(): Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseURL).pipe(take(1));
  }

  public getEventosByTema(tema: string): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/${tema}/tema`).pipe(take(1));
  }

  public getEventoById(id: number): Observable<Evento> {
    return this.http.get<Evento>(`${this.baseURL}/${id}`).pipe(take(1));
  }

  public post(evento: Evento): Observable<Evento> {
    return this.http.post<Evento>(this.baseURL, evento).pipe(take(1));
  }

  public put(evento: Evento): Observable<Evento> {
    return this.http.put<Evento>(`${this.baseURL}/${evento.id}`, evento).pipe(take(1));
  }

  public deleteEvento(id: number): Observable<any> {
    return this.http.delete<string>(`${this.baseURL}/${id}`).pipe(take(1));
  }

  postUpload(eventoId: number, file: File): Observable<Evento> {
    const fileToUpLoad = file as File;
    const formData = new FormData();
    formData.append('file', fileToUpLoad);
    return this.http.post<Evento>(`${this.baseURL}/upload-image/${eventoId}`, formData).pipe(take(1));
  }

}
