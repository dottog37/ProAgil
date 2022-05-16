import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PaginatedResult } from '@app/models/Pagination';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { Evento } from '../models/Evento';

@Injectable(
  //{providedIn: 'root'}
  )
export class EventoService {

  constructor(private http: HttpClient) { }

  baseURL = environment.apiURL + 'api/eventos';


  public getEventos(page?: number, itemsPerPage?: number): Observable<PaginatedResult<Evento[]>> {
    const paginatedResult: PaginatedResult<Evento[]> = new PaginatedResult<Evento[]>();

    let params = new HttpParams;

    if (page != null && itemsPerPage != null){
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    return this.http
    .get<Evento[]>(this.baseURL, {observe: 'response', params})
    .pipe
    (take(1),
      map((response) =>{
        paginatedResult.result = response.body;
        if (response.headers.has('Pagination')){
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination')!);
        }
        return paginatedResult;

      }));
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
