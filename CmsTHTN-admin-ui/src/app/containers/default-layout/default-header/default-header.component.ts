import { Component, Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

import { ClassToggleService, HeaderComponent } from '@coreui/angular';
import { UrlConstants } from '../../../shared/constants/url.constants';
import { TokenStorageService } from '../../../shared/services/token-storage.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-default-header',
  templateUrl: './default-header.component.html',
})
export class DefaultHeaderComponent extends HeaderComponent {

  @Input() sidebarId: string = "sidebar";

  public newMessages = new Array(4)
  public newTasks = new Array(5)
  public newNotifications = new Array(5)
  public username: string = '';

  constructor(private classToggler: ClassToggleService,
    private tokenService: TokenStorageService,
    private router: Router
  ) {
    super();
  }

  ngOnInit(): void {
    const user = this.tokenService.getUser(); // Giả sử getUser() trả về { username: 'Nguyễn' }
    if (user && user.firstName) {
      this.username = user.firstName;
    }
  }

  logout(){
    this.tokenService.signOut();
    this.router.navigate([UrlConstants.LOGIN]);
  }

}
