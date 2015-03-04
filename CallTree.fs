namespace Charities

open System.Collections

module Types =

    type RequestedNeeds =
        | Legal
        | Dental
        | Medical
        | TraumaTherapy
        | EconomicSupport
        | Housing
        | SkillsTraining
        | EducationalHelp
        | JobPlacement

    type Caller =
        | Victim
        | Survivor
        | Advocate

    type NgoType =
        | VictimSafehouse
        | HomelessShelter
        | PovertyRelief
        | MedicalDentalCare
        | SurvivorAid

    type Ngo = Ngo of NgoType * string

    type Followup =
        | NotFollowedUp
        | FollowedUp of Help
        | CallerSelfFollow of Help
    and Help =
        | Helped // got helped with everything needed
        | RanOutOfHelps //exhausted options and still not helped
        | NotHelped of Followup //not helped and not given a referral
        | WrongHelp of Followup // offered something but NOT the help needed
        | Referral of CallerRefToNextNgo //not helped and referred to next NGO
    and CallerRefToNextNgo = CallerRefToNextNgo of Followup * Ngo

    type PoliceDisp =
        | VictimRescued
        | CopsNoHelp of Followup

    type CallOutcome =
        | ProvideDirectHelpToVictimOrSurvivor
        | EmergencyResponse of PoliceDisp
        | CallerRef of CallerRefToNextNgo
        | DisconnectCall of Followup
        | FailedToHelpCaller of Followup

    type CallerRequest =
        | PoliceDispatch
        | VictimServices //immediate safebed until victim gets into aftercare program
        | SurvivorAssistance of Set<RequestedNeeds> //aftercare

    type Call = Call of Caller * CallerRequest * CallOutcome


