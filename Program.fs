// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.
open System
open System.IO
open System.Data
open System.Data.SqlClient
open System.Data.Linq
open Microsoft.FSharp.Data
open Newtonsoft.Json
open Charities.Types
open Charities.TerminalBuilder

let rec ngoinfo(Ngo(cat, name)) =
    match cat with
    | VictimSafehouse -> printfn "Victim Safehouse: %s" name
    | HomelessShelter -> printfn "Homeless Shelter: %s" name
    | PovertyRelief -> printfn "Poverty Relief: %s" name
    | MedicalDentalCare -> printfn "Medical/Dental Charity Clinic: %s" name
    | SurvivorAid -> printfn "Survivor Aftercare: %s" name

let rec help(h:Help) =
    match h with
    | Helped -> printfn "Got all the help needed"
    | RanOutOfHelps -> printfn "Exhausted all options and still not helped"
    | NotHelped (fu) -> printfn "Denied help (possible discrimination?)"
                        followupper(fu)
    | WrongHelp (fu) -> printfn "Offered help, but not the help I needed with my situation"
                        followupper(fu)
    | Referral (crngo) -> printfn "Not helped and referred to the next charity/NGO"
                          callerreftonextngo(crngo)

and callerreftonextngo(CallerRefToNextNgo(fu, ng)) =
    ngoinfo(ng)
    followupper(fu)

and followupper(fu:Followup) =
    match fu with
    | NotFollowedUp -> printfn "No one followed up/I didn't know what else to do"
    | FollowedUp (h) -> printfn "A caseworker/social worker followed up"
                        help(h)
    | CallerSelfFollow (h) -> printfn "Caller did own followup/no caseworker following up"
                              help(h)

let outcome_of_poldisp(pd) =
    match pd with
    | VictimRescued -> printfn "Victim rescued and taken to a safehouse"
    | CopsNoHelp(fu) -> printfn "Cops no-show/victim not helped"
                        followupper(fu)

//This will have to be changed if we change the corresponding 
//constructor function in TerminalBuilder.fs, call_outcome() so that 
//Polaris is not is not the default first NGO a survivor calls.
let call_out(co:CallOutcome) =
    match co with
    | ProvideDirectHelpToVictimOrSurvivor -> printfn "Polaris helped victim/survivor"
    | EmergencyResponse(pd) -> printfn "Hotline operator dispatched 911"
                               outcome_of_poldisp(pd)
    | CallerRef(crngo) -> printfn "Got referred to another NGO"
                          callerreftonextngo(crngo) 
    | DisconnectCall(fu) -> printfn "Call got dropped/disconnected"
                            followupper(fu)
    | FailedToHelpCaller (fu) -> printfn "Was not helped or given any referral"
                                 followupper(fu)
                        

let fx(rn:Set<RequestedNeeds>) =
    Seq.iter(fun x ->
        match x with
        | Legal -> printfn "Legal"
        | Dental -> printfn "Dental"
        | Medical -> printfn "Medical"
        | TraumaTherapy -> printfn "Trauma Therapy"
        | EconomicSupport -> printfn "Economic Support"
        | Housing -> printfn "Housing"
        | EducationalHelp -> printfn "Educational Help"
        | SkillsTraining -> printfn "Skills Training"
        | JobPlacement -> printfn "Job Placement") (rn)

let caller_req(cr:CallerRequest) =
    match cr with
    | PoliceDispatch -> printfn "911 dispatched to rescue victim"
    | VictimServices -> printfn "Victim Services"
    | SurvivorAssistance(rn) -> printfn "Survivor Aid"
                                fx(rn)

let caller_info(c:Caller) =
    match c with
    | Victim -> printfn "Victim"
    | Survivor -> printfn "Survivor"
    | Advocate -> printfn "Advocate"

let call_info(Call(ca, cr, co))=
    printfn "********************************"
    printfn "*   Help Request Report        *"
    printfn "********************************"
    caller_info(ca)
    caller_req(cr)
    call_out(co)



[<EntryPoint>]
let main argv = 
    let c = (caller())
    let cr = (callerRequest())
    let co = (call_outcome())
    let ca = Call(c, cr, co)

    let js = JsonConvert.SerializeObject(ca)

    use w = new StreamWriter("report.json", false)
    w.Write(js)

    let ca2 = JsonConvert.DeserializeObject<Call>(js) 
    call_info(ca2)


    0 // return an integer exit code

