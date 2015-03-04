namespace Charities

module TerminalBuilder =

    open System
    open System.IO
    open Charities.Types

    let rec caller():Caller =
        printfn "Are you reporting as a victim, survivor, or advocate?"
        printfn " 1 for Victim"
        printfn " 2 for Survivor"
        printfn " 3 for Advocate"
        let response = Console.ReadLine()
        match response with
        | "1" -> Victim
        | "2" -> Survivor
        | "3" -> Advocate
        | _ -> printfn "Invalid Entry"
               caller()

    let rec ngo():Ngo =
        printfn "What kind of NGO were you referred to?"
        printfn " 1 for Sex Trafficking Victim Safehouse"
        printfn " 2 for Emergency Homeless Shelter"
        printfn " 3 for Poverty Relief"
        printfn " 4 for Medical and Dental Care"
        printfn " 5 for General Survivor Aid and Aftercare"
        let response = Console.ReadLine()
        printfn "Enter then name of the NGO you were referred to: "
        let resp = Console.ReadLine()
        printfn "%s" resp
        match response.Trim().ToLower() with
        | "1" -> Ngo(VictimSafehouse, resp)
        | "2" -> Ngo(PovertyRelief, resp)
        | "3" -> Ngo(HomelessShelter, resp)
        | "4" -> Ngo(MedicalDentalCare, resp)
        | "5" -> Ngo(SurvivorAid, resp)
        | _ -> 
            printfn "Invalid Entry"
            ngo()

    let needs():Set<RequestedNeeds> =
        let rec nds(s:Set<RequestedNeeds>):Set<RequestedNeeds> =
            printfn "What following unmet needs did you seek help for?"
            printfn "=========================================================="
            printfn "List of Unmet Needs"
            printfn "=========================================================="
            printfn "Legal, Dental, Medical, Trauma Therapy, Economic Support,"
            printfn "Housing, Skills Training, Educational Help, Job Placement"
            printfn "----------------------------------------------------------"
            printfn "\r"
            printfn "Enter one need at a time, then hit 'Enter' after each need."
            printfn "Type 'Done' and hit 'Enter' when finished."
            let response = Console.ReadLine()
            match response.Trim().ToLower() with
            | "done" -> s
            | _ ->
                let n =
                    match response.Trim().ToLower() with
                    | "legal" -> Some Legal
                    | "dental" -> Some Dental
                    | "medical" -> Some Medical
                    | "trauma therapy" -> Some TraumaTherapy
                    | "economic support" -> Some EconomicSupport
                    | "housing" -> Some Housing
                    | "educational help" -> Some EducationalHelp
                    | "job placement" -> Some JobPlacement
                    | _ -> printfn "Invalid Entry"
                           None
                match n with
                | None -> nds(s)
                | Some(x) -> nds(s.Add(x))
        nds(new Set<RequestedNeeds>([]))


    let rec callerRequest():CallerRequest =
        printfn "What is the help you sought?" 
        printfn " 1 for Victim Services" //emergency safehouse bed until program placement
        printfn " 2 for Survivor Aftercare" //
        printfn " 3 for Police Response to Trafficking Situation in progress"
        let answer = Console.ReadLine()
        match answer with
        | "1" -> VictimServices
        | "2" -> SurvivorAssistance (needs())
        | "3" -> PoliceDispatch
        | _ -> printfn "Invalid Entry"
               callerRequest()
    
    let rec followup() =
        printfn "Did anyone follow up with you?"
        printfn " 1 for No (and I didn't know what to do)"
        printfn " 2 for Yes (a caseworker/social worker said they would follow up"
        printfn " 3 for Self-followup/no followup from caseworker"
        let reply = Console.ReadLine()
        match reply with
        | "1" -> NotFollowedUp
        | "2" -> FollowedUp (helpbuilder())
        | "3" -> CallerSelfFollow (helpbuilder())
        | _ -> printfn "Invalid Entry"
               followup()

    and helpbuilder():Help =
        printfn "What help did you get?"
        printfn " 1 if you got all the help you needed/requested"
        printfn " 2 if you were not helped and all referrals were exhausted"
        printfn " 3 if you were denied help and not given a referral"
        printfn " 4 if you were offered the WRONG help"
        printfn " 5 if you were not helped but given a referral to another NGO"
        let response = Console.ReadLine()
        match response with
        | "1" -> Helped
        | "2" -> RanOutOfHelps
        | "3" -> NotHelped (followup())
        | "4" -> WrongHelp (followup())
        | "5" -> Referral (refbuilder())
        | _ -> printfn "Invalid Entry"
               helpbuilder()

    and refbuilder():CallerRefToNextNgo =
        let n = ngo()
        let f = followup()
        CallerRefToNextNgo (f, n)

    let rec police_disp():PoliceDisp =
        printfn "Did police help victims without arresting her/him (Y or N)?"
        let reply = Console.ReadLine()
        match reply with
        | "Y" -> VictimRescued
        | "N" -> CopsNoHelp (followup())
        | _ -> printfn "Invalid Entry"
               police_disp()

// Here is where we may have to change the code so that Polaris is not
//   the default first NGO a victim/survivor calls when seeking help 
    let rec call_outcome():CallOutcome =
        printfn "What help did Polaris provide?"
        printfn " 1 for Polaris provided aftercare to survivor"
        printfn " 2 for hotline operator dispatched 911"
        printfn " 3 for Polaris referred me to another NGO"
        printfn " 4 for Polaris did not help me at all"
        printfn " 5 for call to anti-trafficking hotline got disconnected"
        let reply = Console.ReadLine()
        match reply with
        | "1" -> ProvideDirectHelpToVictimOrSurvivor
        | "2" -> EmergencyResponse (police_disp())
        | "3" -> CallerRef (refbuilder())
        | "4" -> FailedToHelpCaller (followup())
        | "5" -> DisconnectCall (followup())
        | _ -> printfn "Invalid Entry"
               call_outcome()






   

    

    

    










