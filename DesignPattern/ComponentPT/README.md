# ✍️ 컴포넌트 패턴
컴포넌트 패턴이란 로직을 기능별로 나눠서(모듈화) 개발하는 것이다.

 

게임 개발을 예로 들면 캐릭터의 렌더링, 사운드, 충돌 등 모든 기능이 한 클래스 안에 있다면 사소한 기능을 추가하거나 수정해도 버그가 발생할 확률이 매우 높다. 심지어 하나의 버그를 고치면 더 많은 버그가 발생할 수도 있다.  

즉, 한 클래스 안에 모든 코드를 작성하면 기능 간 의존도가 높아지는 커플링 현상이 발생하게 된다. 

 

이러한 문제를 해결하기 위해 컴포넌트 패턴이 등장했는데 여러 기능을 수행할 개체(컨테이너)에 기능별로 분리해서 작성한 컴포넌트를 붙여가며 전체적인 로직을 완성해간다.

대표적으로 유니티가 컴포넌트 패턴을 사용하는 좋은 예시인데, Empty Object(컨테이너)를 하나 생성하고 여기에 스크립트, 오디오 리스너, 메시 렌더러와 같은 컴포넌트를 붙여 객체가 필요한 기능들을 하나씩 추가해간다. 

 

# 🍊 유니티 실습
아래는 캐릭터가 회전하고 이동하고 0.5초 기다리는 작업을 한 클래스에 작성한 코드다.

```cpp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
enum Move
{
    Right,
    Left
}
 
public class Spaghetti : MonoBehaviour
{
    GameObject _player1;
    GameObject _player2;
    Move _move = Move.Right;
 
    void Start()
    {
        _player1 = GameObject.Find("Player1");
        _player2 = GameObject.Find("Player2");
 
        StartCoroutine(MixedAct());
    }
 
    IEnumerator MixedAct()
    {
        while(true)
        {
            // 회전
            _player1.transform.Rotate(90.0f * Vector3.up);
            _player2.transform.Rotate(90.0f * Vector3.up);
 
            if (_player1.transform.position.x < -4)
                _move = Move.Right;
            else if (_player1.transform.position.x > 4)
                _move = Move.Left;
 
            // 이동
            if(_move == Move.Right)
            {
                _player1.transform.Translate(1.0f * Vector3.right, Space.World);
                _player2.transform.Translate(-1.0f * Vector3.right, Space.World);
            }
            else if(_move == Move.Left)
            {
                _player1.transform.Translate(-1.0f * Vector3.right, Space.World);
                _player2.transform.Translate(1.0f * Vector3.right, Space.World);
            }
 
            yield return new WaitForSeconds(0.5f);
        }
    }
}
```
![](./images/스냅샷1.gif)

만약 회전은 0.3초마다 이동은 0.5초마다 하고 싶다면 어떻게 해야 할까?

지금은 로직이 간단해서 수정이 쉽겠지만 기능이 많이 추가된 상태라면 이동과 회전에 관련된 코드를 찾는 것조차 고된 작업이 될 것이다.

그렇다면 회전과 이동을 두 개의 컴포넌트로 분리해서 관리하면 장기적으로 코드 유지 보수가 쉬워질 것이다.


<br><b>0.5초 이동 컴포넌트</b>
```cpp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class MoveAct : MonoBehaviour
{
    enum Dir
    {
        Right,
        Left
    }
 
    Dir _move = Dir.Right;
 
    void Start()
    {
        StartCoroutine(Move());    
    }
 
    IEnumerator Move()
    {
        while (true)
        {
            if (gameObject.transform.position.x < -4)
                _move = Dir.Right;
            else if (gameObject.transform.position.x > 4)
                _move = Dir.Left;
 
            // 이동
            if (_move == Dir.Right)
                gameObject.transform.Translate(1.0f * Vector3.right, Space.World);
            else if (_move == Dir.Left)
                gameObject.transform.Translate(-1.0f * Vector3.right, Space.World);
 
            yield return new WaitForSeconds(0.5f);
        }
    }
}
```

<br><b>0.3초 회전 컴포넌트</b>
```cpp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class RotateAct : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Rotate());
    }
 
    IEnumerator Rotate()
    {
        while(true)
        {
            transform.Rotate(90.0f * Vector3.up);
 
            yield return new WaitForSeconds(0.3f);
        }
    }
}
```

# 👌 결론
코드가 방대하더라도 요구사항이 변했을 때 대처가 빠르다는 장점이 있다!

 

대표적으로 유니티는 기능을 컴포넌트 단위로 나눔으로써 상속의 복잡성을 피하고 캡슐화를 촉진한다. <br>
왜? 상속은 공통 기능을 고려해 조상 클래스를 설계해야 하고 자식 클래스가 확장될 방향도 고려해야 한다.

반면 컴포넌트 패턴은 기능을 분리하는 것이 핵심으로 상속보다 직관적이며 복잡도가 낮다.
