import { User } from "./user";

export class Match {
    public id: number;
    public tournamentId: number;
    public round: number;
    public score: string;
    public player1Id: string;
    public player2Id: string;
    public winnerId: string;
    public looserId: string;
}
