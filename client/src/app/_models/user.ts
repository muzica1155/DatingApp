export interface User // interface in typescript are a little bit different to interface in C# when we use an interface 
//when  we use interface in typescript  we can use it to specify that smthing is a type of smthing 
//now in this case we r going to have a user name , which is going to be a string & we r going 
// give interface teo properties 
{
    username: string;
    token: string;
    photoUrl: string;//property 
    knownAs: string;//we haven't used this property anywhere add in navbar
    gender: string;

}

//user interface has username & a token property does give us is Intellisense in our application as well 
    //typescript saves us from making silly mistakes 

//when we r using typescript we r going to rely on typescript inferring what properties are
//

// let data: number | string = 42;

// //supposig we want to change data later on & set it to ten for instance 
// data = "10";
//if we want to ASSIGN EITHER a number or for whatever reason we want a property or a variable to have it 
//add a colon than specify that data is a number or we can add the pipe symbol & say that data can be a string 
//add if we add quote, marks around 10 then we dont get any complaints bcoz data can now either bw a string
//or it can be a number & its going to be eithere of those


//lets create a couple og objects 

// interface Car{
//     color: string;
//     model: string;
//     topSpeed?: number; //if topSpeed is optional use topSpeed?: number;//no error but its optional doesn't means that we can change it into a string we r going to get a complaint '
// }
// const carl: Car = {//property topSpeed is missign in our car 
//     color: 'blue', // ERROR BCOZ WE HAVE TYPE SAFETY INSIDE OUR OBJECTS 3 is not a string 
//     model: 'BMW'
// }// what we typically do as we didi with our user is we would create an interface bcoz want all of our car ojects to be consistent
// //

// const car2 = {
//     color: 'red',
//     model: 'Mercedes',
//     topSpeed: 100 //even its optional we cant change it into a string// topSpeed: '100'//
// }//we going to get a complaint
// //in typescript  we can use type safety inside functions


// const multiple = (x, y) => {
//     return x * y;// we get a wARNING in the code ///warnign say i will wait & see what this are but t does know this is going to return a number and  
// }

// but if we tried to assign x as a string then typescript is going to know that our methods are going 
//have a prob bcoz x must be of time
//bcoz x is type any number begins or an enum type it cannot be a string we cannot multiply a string by a number .
// const multiple = (x:string, y:number) => {//= (x:number, y:number) => {// but we have to make this a number or we explicit and say we r going to amke this or enforce this that the fact this is a number 
//   return x * y;
// }

// const multiple = (x:number, y:number) => {
//     x * y; // if we dont return anything then typescript is going to infer that this is going to return void 
//   }


// const multiple = (x:number,
//      y:number): void  => { //we can be explicit about this if we want to returning 
//     x * y; 
//   }

//   //Typescript 