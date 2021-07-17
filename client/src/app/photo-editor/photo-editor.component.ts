import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { Photo } from '../_models/photo';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { MemberService } from '../_services/member.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() member : Member;
  uploader : FileUploader;
  hasBaseDropZoneOver = false;
  user : User;
  baseUrl = environment.apiUrl;
  
  constructor(private accountservice : AccountService, private memberService : MemberService) { 
    this.accountservice.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e : any){
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader(){
    this.uploader = new FileUploader({
      url : this.baseUrl + 'users/photo',
      authToken: 'Bearer ' +this.user.token,
      isHTML5 : true,
      autoUpload: false,
      allowedFileType : ['image'],
      removeAfterUpload : true,
      maxFileSize : 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) =>{
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (file,response,status,headers) =>{
      if(response){
        const photo : Photo = JSON.parse(response);
        this.member.photos.push(photo);
        if(photo.isMain){
          this.user.photoUrl = photo.url;
          this.member.mainPhotoUrl = photo.url;
          this.accountservice.setCurrentUser(this.user);
        }
      }
    }

  }

  setMain(photo : Photo){
    this.memberService.setMain(photo.id).subscribe(() =>{
      this.user.photoUrl = photo.url;
      this.accountservice.setCurrentUser(this.user);
      this.member.mainPhotoUrl = photo.url;
      this.member.photos.forEach(p =>{
        if(p.isMain) p.isMain = false;
        if(p.id === photo.id) p.isMain = true;
      })

    });
  }

  deletePhoto(photoId : number){
    this.memberService.deletePhoto(photoId).subscribe(()=>{
      this.member.photos = this.member.photos.filter(photo => photo.id != photoId);
    })
  }

}
