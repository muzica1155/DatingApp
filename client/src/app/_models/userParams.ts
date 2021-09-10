import { User } from "./user";

export class UserParams // this way we give it intial parameters that we can use when we instantiates any instance of this class & specify the agenda is a type of string & we'll 
{
    gender:string; //
    minAge = 18;
    maxAge = 99;// bcoz we will be displaying this info to the user when they load the members list 
    // we r going to display what a default currently r inside here is
    pageNumber = 1;
    pageSize = 5; //
    orderBy = 'lastActive';//add a property for orderBy the default that we set on the server was last active also set this by default to last active 

    constructor(user:User){
        this.gender = user.gender === 'female' ? 'male' : 'female';//ender === 'female' ?//tenary operator 
        //make life more easier now we can remove some of the code that we 've got inside that we currently using 
    }
}