import { Component, ViewEncapsulation } from '@angular/core';
import { AuthService } from '@app/_services';
import { DashboardService } from '../../_services/dashboard.service';
import { CardSettingsModel, SwimlaneSettingsModel, DialogSettingsModel, DialogEventArgs, DragEventArgs } from '@syncfusion/ej2-angular-kanban';
import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { Chart } from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class DashboardComponent {
  canvas: any;
  ctx: any;
  canvas1: any;
  ctx1: any;
  canvas2: any;
  ctx2: any;
  canvas3: any;
  ctx3: any;
  canvas4: any;
  ctx4: any;
  canvas5: any;
  ctx5: any;
  constructor(
    private dashboardService: DashboardService
  ) { }
  ds_list_moi: any = [];
  ds_list_choxl: any = [];
  dang_xuli: number = 0;
  moi_tiepnhan: number = 0;
  xuli_dunghan: number = 0;
  xuli_trehan: number = 0;
  ngOnInit(): void {
    this.canvas = document.getElementById('tientrinh_hoanthanh');
    this.ctx = this.canvas.getContext('2d');
    this.canvas1 = document.getElementById('tiendo_nhanvien');
    this.ctx1 = this.canvas1.getContext('2d');
    this.canvas2 = document.getElementById('tiendo_thuchien');
    this.ctx2 = this.canvas2.getContext('2d');
    this.canvas3 = document.getElementById('conviec_radar');
    this.ctx3 = this.canvas3.getContext('2d');
    this.canvas4 = document.getElementById('conviec_trungtam');
    this.ctx4 = this.canvas4.getContext('2d');
    this.canvas5 = document.getElementById('phantram');
    this.ctx5 = this.canvas4.getContext('2d');
    let tiendo_thuchien = new Chart(this.ctx2, {
      type: 'doughnut',
      data: {
        labels: ["Đang thực hiện", "Đã hoàn thành", "Chưa thực hiện"],
        datasets: [{
          label: 'Tiến độ hoàn thành công việc',
          data: [95, 35, 56],
          backgroundColor: ["#42A5F5", "#66BB6A", "#cc6699"],
          datalabels: {
            color: '#fff'
          }
        }]
      },
      options: {
        responsive: true,
        plugins: [ChartDataLabels],
      }
    });
    let tientrinh_hoanthanh = new Chart(this.ctx, {
      type: 'line',
      data: {
        labels: ["31/10/2021", "07/11/2021", "14/11/2021", "21/11/2021", "27/11/2021"],
        datasets: [{
          label: 'Hoàn thành',
          data: [10, 7, 8, 12, 15],
          borderColor: 'green',
        }, {
          label: 'Đang thực hiện',
          borderColor: 'red',
          data: [6, 15, 6, 8, 11],
        }]
      },
      options: {
        spanGaps: true,
        responsive: true,
        tooltips: {
          mode: 'index',
          intersect: false,
        },
        hover: {
          mode: 'nearest',
          intersect: true
        },
        scales: {
          xAxes: [{
            display: true,
          }],
          yAxes: [{
            display: true,
            ticks: {
              min: 1,
              max: 25,

            }
          }]
        }
      }
    });
    var tiendo_nhanvien = new Chart(this.ctx1, {
      type: 'horizontalBar',
      data: {
        labels: ["Bùi Hương Đông", "Trần Văn Thi", "Lê Việt Tuấn", "Phan Trình", "Trần Sỹ Chung"],

        datasets: [{
          label: 'Quá hạn',
          data: [23, 34, 32, 53, 23],
          backgroundColor: "rgb(242,6,103)",
          hoverBackgroundColor: "rgba(50,90,100,1)"
        }, {
          label: 'Đang thực hiện',
          barThickness: 25,
          data: [12, 21, 13, 25, 42],
          backgroundColor: "rgb(49,225,115)",
          hoverBackgroundColor: "rgba(140,85,102,1)"
        }, {
          label: 'Chưa xử lí',
          data: [32, 24, 24, 14, 24],
          backgroundColor: "rgba(63,203,226,1)",
          hoverBackgroundColor: "rgba(46,185,235,1)"
        }]
      },

      options: {
        tooltips: {
          enabled: false
        },
        hover: {
          animationDuration: 0
        },
        scales: {
          xAxes: [{
            barPercentage: 2,
            barThickness: 25,
            maxBarThickness: 25,
            minBarLength: 10,
            ticks: {
              beginAtZero: true,
              fontFamily: "'Open Sans Bold', sans-serif",
              fontSize: 11
            },
            scaleLabel: {
              display: false
            },
            gridLines: {
            },
            stacked: true
          }],
          yAxes: [{
            gridLines: {
              display: false,
              color: "#fff",
              zeroLineColor: "#fff",
              zeroLineWidth: 0
            },
            ticks: {
              fontFamily: "'Open Sans Bold', sans-serif",
              fontSize: 11
            },
            stacked: true
          }]
        },
        legend: {
          display: true
        }
      },
    });
    var conviec_trungtam = new Chart(this.ctx4, {
      type: 'bar',
      data: {
        labels: ["KT-KH", "NS-TH", "KT-ĐT", "ĐHTT", "CNTT", "A Lưới", "Phong Điền", "Nam Sông Hương", "Bắc Sông Hương", "Hương Phú", "Hương Điền"],

        datasets: [{
          label: 'Quá hạn',
          data: [23, 34, 32, 53, 23, 23, 34, 32, 53, 23, 32],
          backgroundColor: "rgb(242,6,103)",
          hoverBackgroundColor: "rgba(50,90,100,1)"
        }, {
          label: 'Đang thực hiện',
          barThickness: 25,
          data: [12, 21, 13, 25, 42, 12, 21, 13, 25, 42, 34],
          backgroundColor: "rgb(49,225,115)",
          hoverBackgroundColor: "rgba(140,85,102,1)"
        }, {
          label: 'Chưa xử lí',
          data: [32, 24, 24, 14, 24, 32, 24, 24, 14, 24, 35],
          backgroundColor: "rgba(63,203,226,1)",
          hoverBackgroundColor: "rgba(46,185,235,1)"
        }]
      },

      options: {
        tooltips: {
          enabled: true
        },
        hover: {
          animationDuration: 0
        },
        scales: {
          xAxes: [{
            barPercentage: 2,
            barThickness: 25,
            maxBarThickness: 25,
            minBarLength: 10,
            ticks: {
              beginAtZero: true,
              fontFamily: "'Open Sans Bold', sans-serif",
              fontSize: 11
            },
            scaleLabel: {
              display: false
            },
            gridLines: {
            },
            stacked: true
          }],
          yAxes: [{
            gridLines: {
              display: false,
              color: "#fff",
              zeroLineColor: "#fff",
              zeroLineWidth: 0
            },
            ticks: {
              fontFamily: "'Open Sans Bold', sans-serif",
              fontSize: 11
            },
            stacked: true
          }]
        },
        legend: {
          display: true
        }
      },
    });
    let conviec_radar = new Chart(this.ctx3, {
      type: 'radar',
      data: {
        labels: ["Phòng KH-KT", "Phòng NS-TH", "Phòng KT-ĐT", "ĐHTT", "CNTT", "A Lưới", "Phòng Điền", "Nam Sông Hương", "Bắc Sông Hương", "Hương Phú", "Hương Điền"],
        datasets: [{
          label: "Tổng số công việc",
          backgroundColor: "rgba(200,0,0,0.2)",
          data: [200, 150, 170, 120, 160, 200, 150, 170, 120, 160, 120],
          borderColor: 'red',
        }, {
          label: "Đã hoàn thành",
          backgroundColor: "rgba(0,0,200,0.2)",
          data: [200, 90, 120, 95, 160, 150, 90, 120, 74, 135, 100],
          borderColor: 'green',
        }]
      },
      options: {
        tooltips: {
          enabled: true
        },
        hover: {
          animationDuration: 300
        }
      }
    });
  }

}
