# __<span style="color:#D57500">MTDB Creator</span>__
This software can be used to generate an Accurate Mass and Time tag database from local MS/MS search engine results from MSGF+, X!Tandem, or SEQUEST.  It can create the database in two formats:

* Microsoft Access (.mdb file), compatible with VIPER
* SQLite (.mtdb), compatible with MultiAlign

### Description
The database can act as input to [VIPER](https://pnnl-comp-mass-spec.github.io/VIPER/) and [MultiAlign](https://pnnl-comp-mass-spec.github.io/MultiAlign/) for matching high resolution LC-MS features to peptides.

Example data files are available as a two-part download (see downloads), totaling ~130 MB (ZIP). These files contain example .mzid files from MSGF+, plus two databases created by MTDB Creator: an Access database (.mdb) and a SQLite database (.mtdb). The .mzid files come from the analysis of several quality assessment (QC_Shew) datasets from a Thermo Q-Exactive mass spectrometer. The file also has DeconTools _isos.csv files, which can be processed with MultiAlign or VIPER

To process X!Tandem results with MTDB Creator, they must first be converted to a tab-delimited format using the [Peptide Hit Results Processor](https://pnnl-comp-mass-spec.github.io/PHRP/) application.

MSGF+ saves results as .mzid files. MTDB Creator can directly read .mzid or .mzid.gz files created by MSGF+

The "[Data Extraction and Analysis for LC-MS Based Proteomics](https://panomics.pnnl.gov/training/workshops/)" sessions at the 2007 and 2008 US HUPO conferences discussed the use of the MTDB Creator to create AMT tag databases. PDF files of the slides presented are available for download as a [5 MB PDF file (2007)](http://panomics.pnnl.gov/training/workshops/2007HUPO/LCMSBasedProteomicsDataProcessing.pdf) and a [6.5 MB PDF file (2008)](http://panomics.pnnl.gov/training/workshops/2008HUPO/LCMSBasedProteomicsDataProcessing2008.pdf).

### Downloads
* [Latest version](https://github.com/PNNL-Comp-Mass-Spec/MTDB-Creator/releases/latest)
* [Source code on GitHub](https://github.com/PNNL-Comp-Mass-Spec/MTDB-Creator)
* Example input files for MTDB Creator (extract both files to the same directory)
  * [Example files part 1](MTDB_Creator_Example_Data_1.zip)
  * [Example files part 2](MTDB_Creator_Example_Data_2.zip)

#### Software Instructions
Download the zip file and extract the contents, then run MTDBCreator.exe

See the PDF for usage instructions.

### Acknowledgment

All publications that utilize this software should provide appropriate acknowledgement to PNNL and the MTDB-Creator GitHub repository. However, if the software is extended or modified, then any subsequent publications should include a more extensive statement, as shown in the Readme file for the given application or on the website that more fully describes the application.

### Disclaimer

These programs are primarily designed to run on Windows machines. Please use them at your own risk. This material was prepared as an account of work sponsored by an agency of the United States Government. Neither the United States Government nor the United States Department of Energy, nor Battelle, nor any of their employees, makes any warranty, express or implied, or assumes any legal liability or responsibility for the accuracy, completeness, or usefulness or any information, apparatus, product, or process disclosed, or represents that its use would not infringe privately owned rights.

Portions of this research were supported by the NIH National Center for Research Resources (Grant RR018522), the W.R. Wiley Environmental Molecular Science Laboratory (a national scientific user facility sponsored by the U.S. Department of Energy's Office of Biological and Environmental Research and located at PNNL), and the National Institute of Allergy and Infectious Diseases (NIH/DHHS through interagency agreement Y1-AI-4894-01). PNNL is operated by Battelle Memorial Institute for the U.S. Department of Energy under contract DE-AC05-76RL0 1830.

We would like your feedback about the usefulness of the tools and information provided by the Resource. Your suggestions on how to increase their value to you will be appreciated. Please e-mail any comments to proteomics@pnl.gov
