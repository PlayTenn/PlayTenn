import { TennisLevel } from "./tennisLevel";

export class User {
 public id!: string;
 public name!: string;
 public surname!: string;
 public email!: string;
 public tennisLevelId?: number;
 public tennisLevel?: TennisLevel;
 public profileImageUrl!: string;
 public profileImageFilename?: string;
}
