# Population simulator

Basic simulator of population growth from *X* individuals. (e.g. Planet or island colonisation with only a few people)

## Getting Started

### Prerequisites

**Visual Studio** (2017+) or **Visual Studio Code** is requied.

### Installing

 1. Clone repository
 2. Open the solution **/Simulator/Simulator.sln**
 3. Restore NuGet packages
 4. Build the solution
 5. Run the project

## Basics

### Entity
*To-Do*

### Gender(s)
*To-Do*

### Age(s)
*To-Do*

### Cycles

The simulation is running in cycles. Each cycle has 4 phases which **must** be called in this order:

 1. Make child
 2. Set partner
 3. Get old
 4. Cycle++

**Description**
Firstly, every female that meets the condition has a chance to have babies (0-5). After children 'creation', single *teen* entities have a chance to find a partner (0-1). In the next step, every entity gets old by the rules. When all phases are completed, the cycle number is increased. 
 
#### 1. phase - Make child
Every entity which meets the conditions, can "create" new entity.

##### Required conditions

**Parents**
- [ ] Are alive
- [ ] Are different gender (Male + Female)
- [ ] Are same generation (*Configurable*)
- [ ] Are **NOT** related (*Configurable*)

**Mother**
- [ ] Is female
- [ ] Has partner
- [ ] Partner is alive
- [ ] Partner is male
- [ ] Is **adult**

**Father**
- [ ] Is male
- [ ] Has partner
- [ ] Partner is alive
- [ ] Partner is female
- [ ] Is **Adult** or is **OldAge**

##### Rules
*To-Do*

#### 2. phase - Set partner

A random number between 0 and 1 is generated. (= Minimal attractiveness) 
##### Rules
- [ ] Is alive
- [ ] Is single (Does **NOT** have partner)
- [ ] Is teen (Adolescence age)
- [ ] Is same generation as *future* partner
- [ ] Is same age as partner
- [ ] Is different gender of partner
- [ ] Has greater or same atractiveness than minimal attractiveness

#### 3. phase - Get old
In *Get old* phase each entity is changed by basic rules.

#### Rules
 - Childhood → Adolescence
 - Adolescence → Adulthood
 - Adulthood → Old age (Exception)
 - Old age → Death

#### Exception
A number between 0 and 0.9 is generated. (= Minimal longevity)
If the entity already *skipped* 'getting older', it gets old now.
If the entity's longevity is greater or same as minimal longevity, it *skips* 'getting older'.  


#### 4. phase - Cycle++
If everything was alright, the current cycle number is increased.

## Built With

- [Visual Studio 2017](https://visualstudio.microsoft.com/) - IDE
- [NuGet](https://www.nuget.org/) - Package manager
- [C#](https://docs.microsoft.com/dotnet/csharp/) - Programming language

## Authors

- **Jiri Falta** - *Main programmer* - [Fjarik](https://github.com/Fjarik)

See also the list of [contributors](https://github.com/Fjarik/Population-simulator/graphs/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.