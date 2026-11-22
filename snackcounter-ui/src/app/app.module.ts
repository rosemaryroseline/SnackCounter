import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { RefillListComponent } from './pages/refill-list/refill-list.component';
import { MyAssignmentsComponent } from './pages/my-assignments/my-assignments.component';
import { PurchasesComponent } from './pages/purchases/purchases.component';
import { MonthlyReportsComponent } from './pages/monthly-reports/monthly-reports.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    RefillListComponent,
    MyAssignmentsComponent,
    PurchasesComponent,
    MonthlyReportsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
