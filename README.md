# software-licensing

## Motivation
Our target is to implement a software, which can send programs from the server to the client and execute it. The connection will be encrypted, to prevent any sniffing.

## Realization

### Client

- Requests the program via a license_key
  - Get public key of the server
    ```json
    GET {host}/public_key
    ```
  - Send request to the server  
    ```json
    GET {host}/program
    {
        "data": {
            "client_public_key":  "asdf",
            "license_key": "AAAA-BBBB-CCCC-DDDD",
        },
        "checksum": "813b91b01edabe5cc00cdd2f78ef9c22"
    }
    ```
    Note: The data is encrypted with the public key of the server.
  - Execute the program
    - Save it to file
    - Load it from memory
    ```json
    {
        "data": [],
        "checksum": "813b91b01edabe5cc00cdd2f78ef9c22"
    }
    ```
    Note: The data is encrypted with the public key of the client.


## Encryption

- Data will be a `byte[]` and because of that we need to find a way to send that. There's multiple options for that: 
  - Encode it as base64 (https://stackoverflow.com/a/52605186/7303868)
  - Use BSON (https://stackoverflow.com/a/32185531/7303868)

## Problems

- MITM in the beginning: Attack is able to spoof the public_key, but he can't get the program.