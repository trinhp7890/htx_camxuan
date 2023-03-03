import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Dashboard_box, Dashboard_CV, Dashboard_CV_bieudo } from '@app/_models/dashboard/dashboard';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
const baseUrl = `${environment.apiURL}/api/thongbao`;
const baseUrl_realtime = `${environment.apiURL_Realtime}`;

@Injectable({
  providedIn: 'root'
})
export class RealtimeService {

  public _hubConnection: HubConnection;
  public connectionId: string;

  constructor(private http: HttpClient) { }



  connect_realtime(UserName): void {
    this._hubConnection = new HubConnectionBuilder().withUrl(`${baseUrl_realtime}/notify`).build();
    this._hubConnection.start()
    .then(()=>
    console.log('connection start'))
    .then(() => this.getConnectionId(UserName))
    .catch(err=>{
      console.log('Error while establishing the connection')
    });
   
    // this._hubConnection.on('BroadcastMessage', (message)=>{
    //   console.log(message);
    //   this.get_all();
    //   //this.getConnectionId();
    // })
  }


  private getConnectionId = (UserName) => {
    this._hubConnection.invoke('getconnectionid')
    .then((data) => {
      this.connectionId = data;
      this.senddata(UserName);
    });
  }
  senddata(UserName){
    var data={
      Type: UserName,
      Information: this.connectionId,
    }
    this.http.post(`${baseUrl_realtime}/api/Msg`, data).subscribe(
      data=>{
        //console.log(data);
      }
    );
  }
  sendmsg(datasend){
    this.http.post(`${baseUrl_realtime}api/Message`, datasend).subscribe(
      data=>{
        //console.log(data);
      }
    );
  }
  sendmsg_congviec(datasend){
    this.http.post(`${baseUrl_realtime}api/Congviec`, datasend).subscribe(
      data=>{
        //console.log(data);
      }
    );
  }

  // get_box(ma: string) {
  //     return this.http.get<Dashboard_box>(`${baseUrl}/viewbox?prmMA_NV=${ma}`);
  // }
  // get_box_donvi(ma: string,child: number) {
  //   return this.http.get<Dashboard_box>(`${baseUrl}/viewbox_donvi?prmMA_DV=${ma}&prmChildNumber=${child}`);
  // }

  get_thongbao_realtime_byma(prmMA_NV: string) {
    return this.http.get<any>(`${baseUrl}/realtimebymanv?prmMA_NV=` + prmMA_NV);
  }
  thongbao_realtime_up(prmIDTHONGBAO: number): Observable<any> {
    return this.http.post(
      `${baseUrl}/realtimeupdate?prmIDTHONGBAO=`+prmIDTHONGBAO,      
      { withCredentials: true }
    );
  }
  // get_chitiet(ma: string) {
  //   return this.http.get<Dashboard_CV>(`${baseUrl}/view_chitiet?prmMA_NV=${ma}`);
  // }
  // get_bieudo(ma: string) {
  //   return this.http.get<Dashboard_CV_bieudo>(`${baseUrl}/view_bieudo?prmMA_NV=${ma}`);
  // }



}
