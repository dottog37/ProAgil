import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';

@Injectable(
  //{providedIn: 'root'}
  )
export class EventoService {

  constructor(private http: HttpClient) { }

  baseURL = 'https://localhost:5001/api/Eventos';

  public getEventos(): Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseURL);
  }

  public getEventosByTema(tema: string): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/${tema}/tema`);
  }

  public getEventoById(id: number): Observable<Evento> {
    return this.http.get<Evento>(`${this.baseURL}/${id}`);
  }

  public post(evento: Evento): Observable<Evento> {
    return this.http.post<Evento>(this.baseURL, evento);
  }

  public put(evento: Evento): Observable<Evento> {
    return this.http.put<Evento>(`${this.baseURL}/${evento.id}`, evento);
  }

  public deleteEvento(id: number): Observable<any> {
    return this.http.delete<string>(`${this.baseURL}/${id}`);
  }

  postUpload(eventoId: number, file: File): Observable<Evento> {
    const fileToUpLoad = file as File;
    const formData = new FormData();
    formData.append('file', fileToUpLoad);
    return this.http.post<Evento>(`${this.baseURL}/upload-image/${eventoId}`, formData);
  }

}
