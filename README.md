# Pingle 
Pingle (Beta) is a computer network administration software utility used to test the propagation speed between a host computer and a client on an Internet Protocol (IP) network. It **measures Inbound, Outbound and inbound download speed** and **issues the results in csv format**. It has a built in benchmark system to check the under test hardware resources of client and server to give a clear overview about the test results. 

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)

The benchmark parameters are as follow:

* Number of clients on the network (Number of active machines on the same network has direct effect on overall bandwidth and your test)
* Processor specification
* Number of cores  
* Number of logical processors 
* Number of processors
* Cache memory size
* RAM memory size
* OS
* System type 64 or 32 bit (for 32 bit you must compile the code yourself)
* DNS host name
* Antivirus vendor
* Firewall vendor
* Machine manufacturer 
* Machine model
* Hard disk drive model SSD, manufacturer
* HDD Read and write speed



It supports Windows operating system from 7 and higher versions. Use it at your own risk.

###### Disclaimer

The software provides “as is” without warranty of any kind, implied or expressed. You assume full responsibility and risk of loss resulting from your data and use of the content of the software. We expressly disclaim any warranty of merchantability, title, security, accuracy and non-infringement. In no event shall the author be liable for any claim, damages or other liability arising from the software or the use of the software. The software is provided as an information resource only, and should not be used or relied on for any diagnostic or treatment purposes.



How to use?

Clone this git repository. Inside the cloned folder there is a setup file, install the setup on both client and server machines (one machine plays the role of server and another client).  You will get two icons on your Desktop. In one machine run the client and in another machine run server. Note that both applications need administrator permission.  



1- Run both applications. 

![](/images/cli_icon.png)

![](/images/serv_icon.png)



###### Server Application:

​	2- Type the port.

![1](/images/1.png)



​	3- Choose your network card from the dropdown list. 

![2](/images/2.png)



​	4- If you want to apply the network download speed limitation then change it according to the  picture. 

![3](/images/3.png)



​	5- Choose the download size. 

![4](/images/4.png)

​	

​	6- Start the server.

![5](/images/5.png)



###### Client Application:

​	7- Enter the IP and Port similar to the server machine.

![6](/images/6.png)



​	8- Click on start to start the test.

![7](/images/7.png)



​	9- Create the report.

![8](/images/8.png)



​	10- Result of test!

![9](/images/9.png)

