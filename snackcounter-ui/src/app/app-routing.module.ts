
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { RefillListComponent } from './pages/refill-list/refill-list.component';
import { MyAssignmentsComponent } from './pages/my-assignments/my-assignments.component';
import { PurchasesComponent } from './pages/purchases/purchases.component';
import { MonthlyReportsComponent } from './pages/monthly-reports/monthly-reports.component';

const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

  { path: 'dashboard', component: DashboardComponent },
  { path: 'refills', component: RefillListComponent },
  { path: 'assignments', component: MyAssignmentsComponent },
  { path: 'purchases', component: PurchasesComponent },
  { path: 'reports', component: MonthlyReportsComponent },

  { path: '**', redirectTo: 'dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
