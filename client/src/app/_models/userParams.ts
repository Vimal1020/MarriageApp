import { User } from "./user";

export class UserParams{
    interestedIn:string;
    minAge = 18;
    maxAge=99;
    pageNumber=1;
    pageSize=5;
    orderBy ='lastActive';

    constructor(user: User){
        this.interestedIn = user.gender === 'female'?'male':'female';
    }
}