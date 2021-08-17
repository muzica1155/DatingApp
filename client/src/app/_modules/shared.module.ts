import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),//when we see forRoot() this means that it's got some services or components that it needs to initialize along with the roots loadsup the services that it needs with our roots modules
    ToastrModule.forRoot
    (
      {positionClass: 'toast-bottom-right'

    }),
    TabsModule.forRoot(),
    NgxGalleryModule,
  ],
  exports: [
    BsDropdownModule,
    ToastrModule,
    TabsModule,
    NgxGalleryModule,
  ]
})
export class SharedModule { }
