import { Photo } from './photo'

export interface Member {
    id: number;
    userName: string;
    mainPhotoUrl: string;
    gender: string;
    age: number;
    city: string;
    country: string;
    dateOfBirth: Date;
    nickName: string;
    bio: string;
    accountCreated: Date;
    lastActive: Date;
    interests: string;
    lookingFor: string;
    photos: Photo[];
}
