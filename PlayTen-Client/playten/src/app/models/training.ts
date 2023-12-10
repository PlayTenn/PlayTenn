import { Place } from "./place";
import { Participant } from "./participant";

export class Training {
 public training!: TrainingInfo;
 public isUserTrainingOwner!: boolean;
 public isUserParticipant!: boolean;
 public isUserApprovedParticipant!: boolean;
 public isUserUndeterminedParticipant!: boolean;
 public isUserRejectedParticipant!: boolean;
 public isTrainingFinished!: boolean;
}

export class TrainingInfo {
 public trainingId?: number;
 public name!: string;
 public dateStart!: Date;
 public dateEnd!: Date;
 public description!: string;
 public placeId!: number;
 public numberOfParticipants!: number;
 public hasBalls!: boolean;
 public ownerId!: string;
 public place?: Place;
 public participants?: Participant[];
}
