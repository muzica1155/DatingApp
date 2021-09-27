export interface Group {
    name: string;
    connections: Connection[]// that's gonna be for type of connections to say interface
}
interface Connection {
    connectionId: string;
    username: string;
}// make use of this in our message server let go to message service 
