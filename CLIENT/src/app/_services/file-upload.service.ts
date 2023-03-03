import { HttpClient, HttpHeaders, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { environment } from '@environments/environment';
import { FileToUpload } from '@app/_models/file-to-upload';
import { catchError, retry } from 'rxjs/operators';
import { Injectable } from '@angular/core';

const baseUrl = `${environment.apiURL}`;
const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json'
    })
};

@Injectable({
    providedIn: 'root'
})
export class FileUploadService {

    constructor(private http: HttpClient) { }

    getToken() {
        let accessToken = localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token') || '';
        if (accessToken !== '') {
            return `Bearer ${accessToken}`;
        } else {
            return '';
        }
    }

    uploadFile(url: string, theFile: FileToUpload): Observable<any> {
        const req = new HttpRequest('POST', `${baseUrl}` + url, theFile, httpOptions);
        return this.http.request(req);
    }

    download(url) {
        return this.http.get(`${baseUrl}` + url, this.getDownloadOptions()).pipe(catchError(this.handleError));
    }

    getDownloadOptions() {
        var token = this.getToken();

        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                Authorization: token,
                'Cache-Control': 'no-cache',
                Pragma: 'no-cache'
            }),
            responseType: 'blob' as 'blob'
        };

        return httpOptions;
    }

    get_all(url : string) {
        return this.http.get<FileToUpload[]>(`${baseUrl}`+url);
    }

    delete(url: string,prmID: number, prmNGUOI_NHAP: string): Observable<any> {
        const req = new HttpRequest('POST', `${baseUrl}`+`/`+ url+`?prmID_FILE=`+ prmID + `&prmNGUOI_NHAP=` + prmNGUOI_NHAP, { withCredentials: true });
        return this.http.request(req).pipe(catchError(this.handleError));
      }

    private handleError(error: HttpErrorResponse) {
        if (error.error instanceof ErrorEvent) {
            console.error('An error occurred:', error.error.message);
        } else {
            console.error(`Backend returned code ${error.status}, ` + `body was: ${error.message}`);
        }
        return throwError(error);
    }
}