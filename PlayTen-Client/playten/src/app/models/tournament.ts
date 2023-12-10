import { Place } from "./place";
import { Participant } from "./participant";

export class Tournament {
 public tournament!: TournamentInfo;
 public isUserTournamentOwner!: boolean;
 public isUserParticipant!: boolean;
 public isUserApprovedParticipant!: boolean;
 public isUserUndeterminedParticipant!: boolean;
 public isUserRejectedParticipant!: boolean;
 public isTournamentFinished!: boolean;
}

export class TournamentInfo {
 public tournamentId?: number;
 public amountOfRounds?: number;
 public name!: string;
 public dateStart!: Date;
 public dateEnd!: Date;
 public description!: string;
 public placeId!: number;
 public numberOfParticipants!: number;
 public ownerId!: string;
 public place?: Place;
 public participants?: Participant[];
 public price?: string;
 public hasStarted?: boolean;
 public finished?: boolean;
}