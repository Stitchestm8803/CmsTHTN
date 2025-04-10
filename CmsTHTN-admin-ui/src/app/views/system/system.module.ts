import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { SystemRoutingModule } from './system-routing.module';
import { UserComponent } from './users/user.component';
import { RoleComponent } from './roles/role.component';
import { TableModule } from 'primeng/table'
import { ProgressSpinnerModule } from 'primeng/progressspinner'
import { BlockUIModule } from 'primeng/blockui'
import { PaginatorModule } from 'primeng/paginator'
import { PanelModule } from 'primeng/panel'
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';
import { RolesDetailComponent } from './roles/roles-detail.component';
import { SharedModule } from 'primeng/api';
import { CmsTHTNSharedModule } from '../../shared/modules/cmsthtn-shared.module';
import { KeyFilterModule } from 'primeng/keyfilter'
import { PermissionGrantComponent } from './roles/permission-grant.component';

@NgModule({
  imports: [
    SystemRoutingModule,
    CommonModule,
    ReactiveFormsModule,
    TableModule,
    ProgressSpinnerModule,
    BlockUIModule,
    PaginatorModule,
    PanelModule,
    CheckboxModule,
    ButtonModule,
    InputTextModule,
    KeyFilterModule,
    SharedModule,
    CmsTHTNSharedModule,
  ],
  declarations: [UserComponent, RoleComponent, RolesDetailComponent, PermissionGrantComponent],
})
export class SystemModule {
}
