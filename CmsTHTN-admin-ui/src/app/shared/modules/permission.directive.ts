import { Directive, ElementRef, Input, OnInit, Renderer2 } from '@angular/core';
import { TokenStorageService } from './../services/token-storage.service';

@Directive({
  selector: '[appPermission]'
})
export class PermissionDirective implements OnInit {
  @Input() appPolicy: string;

  constructor(private el: ElementRef, private renderer: Renderer2, private tokenService: TokenStorageService) {
  }

  ngOnInit() {
    const loggedInUser = this.tokenService.getUser();

    if (!loggedInUser || !loggedInUser.permissions.includes(this.appPolicy)) {
        // **Ẩn ngay từ đầu**
        this.el.nativeElement.style.display = "none";

        // **Xóa hẳn khỏi DOM, kiểm tra nếu `parentNode` tồn tại**
            if (this.el.nativeElement.parentNode) {
            this.renderer.removeChild(this.el.nativeElement.parentNode, this.el.nativeElement);
             }
        }
    }
}