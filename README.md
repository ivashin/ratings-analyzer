# ratings-analyzer

Ratings Analyzer is a demo project that parses online movie rating aggregators (currently only Metacritic is supported), extracts critics' and audience ratings, saves them to a local database and performs analytical queries on extracted ratings.

## Compiling
Use `build.cmd` to compile the project and execute unit tests. Project targets .NET framework version 4.5.1. This framework version should be installed in order to build the project.  
After build, `bin/` directory will contain the build output.

## Usage
Ratings Analyzer is a console app. There are two main modes in which it can operate: `get` and `analyze`.

### Get
This mode extracts ratings from online review aggregators and saves them to a local database.  
Optional key `-r`/`--results` determines how many entries should be extracted (100 by default). Use `0` to load everything.

Example:

    > RatingsAnalyzer.exe get -r 250
### Analyze
This mode runs queries on ratings stored in the database, and saves results to .csv files.  
Currently two query types are supported:
* Use `-u`/`--underrated` to view movies, which were underrated by critics, i.e. have critics ratings lower than audience score.
* Use `-o`/`--overrated` to view movies, which were overrated by critics, i.e. which have audience ratings lower than critics'

Both queries output movies sorted by difference in critics' and audience ratings, with the highest difference being the first.

Required key `-f`/`--file` determines the .csv file with results.  
Optional key `-r`/`--results` controls how many entries will be included in the output (100 by default). Use `0` to output everything.

Examples:

    > RatingsAnalyzer.exe analyze -u -f Underrated.csv
    > RatingsAnalyzer.exe analyze -o -f Overrated.csv -results 10